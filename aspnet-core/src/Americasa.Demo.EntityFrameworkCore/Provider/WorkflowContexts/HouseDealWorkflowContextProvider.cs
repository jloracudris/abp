using Americasa.Demo.Entities.Entities;
using Americasa.Demo.EntityFrameworkCore;
using Elsa.Activities.Http.Models;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Americasa.Demo.Provider.WorkflowContexts
{
    public class HouseDealWorkflowContextProvider : Elsa.Services.WorkflowContextRefresher<HouseDeal>
    {
        //private readonly IDbContextFactory<DemoDbContext> _demoDbContext;
        private readonly IRepository<HouseDeal, Guid> _houseDealsRepository;

        public HouseDealWorkflowContextProvider(
            //IDbContextFactory<DemoDbContext> demoDbContext,
            IRepository<HouseDeal, Guid> houseDealsRepository)
        {
            _houseDealsRepository = houseDealsRepository;
        }

        /// <summary>
        /// Loads a BlogPost entity from the database.
        /// </summary>
        public override async ValueTask<HouseDeal?> LoadAsync(LoadWorkflowContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var houseDealId = context.ContextId;
                //await using var dbContext = _demoDbContext.CreateDbContext();
                //var deals = await _houseDealsRepository.FirstOrDefaultAsync;
                return await _houseDealsRepository.FirstOrDefaultAsync(x => x.InstanceId == houseDealId, cancellationToken);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Updates a BlogPost entity in the database.
        /// If there's no actual workflow context, we will get it from the input. This works because we know we have a workflow that starts with an HTTP Endpoint activity that receives BlogPost models.
        /// This is a design choice for this particular demo. In real world scenarios, you might not even need this since your workflows may be dealing with existing entities, or have (other) workflows that handle initial entity creation.
        /// The key take away is: you can do whatever you want with these workflow context providers :) 
        /// </summary>
        public override async ValueTask<string?> SaveAsync(SaveWorkflowContext<HouseDeal> context, CancellationToken cancellationToken = default)
        {
            var houseDeal = context.Context;
            // await using var dbContext = _demoDbContext.CreateDbContext();
            //var dbSet = dbContext.HouseDeals;

            if (houseDeal == null)
            {
                // We are handling a newly posted blog post.
                houseDeal = ((HttpRequestModel)context.WorkflowExecutionContext.Input!).GetBody<HouseDeal>();

                // Generate a new ID.
                houseDeal.InstanceId = context.WorkflowExecutionContext.WorkflowInstance.Id;

                // Set IsPublished to false to prevent caller from cheating ;)
                houseDeal.IsPublished = false;

                // Set context.
                context.WorkflowExecutionContext.WorkflowContext = houseDeal;
                context.WorkflowExecutionContext.ContextId = houseDeal.InstanceId;

                // Add blog post to DB.
                // await dbSet.AddAsync(houseDeal, cancellationToken);
                try
                {
                    await _houseDealsRepository.InsertAsync(houseDeal, true, cancellationToken);
                }
                catch (Exception e)
                {

                    var test = e;
                }
                
            }
            else
            {
                var houseDealId = houseDeal.InstanceId;
                var existingHouseDealId = await _houseDealsRepository.FirstOrDefaultAsync(x => x.InstanceId == houseDealId, cancellationToken); //await dbSet.AsQueryable().Where(x => x.InstanceId == houseDealId).FirstAsync(cancellationToken);
                await _houseDealsRepository.UpdateAsync(existingHouseDealId, true);
                //_houseDealsRepository.UpdateAsync
                //dbContext.Entry(existingHouseDealId).CurrentValues.SetValues(houseDeal);

            }

            // await dbContext.SaveChangesAsync(cancellationToken);
            return houseDeal.InstanceId;
        }
    }
}
