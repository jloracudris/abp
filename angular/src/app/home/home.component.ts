import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { HouseDealDto } from '@proxy/entities/entities';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { HouseDealsService } from '@proxy/house-deals.service';
import { MatDialog } from '@angular/material/dialog';
import { DealDialogComponent } from './components/deal-dialog/deal-dialog.component';
import { ActionsDialogComponent } from './components/actions-dialog/actions-dialog.component';
import {
  ConfirmDialogComponent,
  ConfirmDialogModel,
} from './components/confirm-dialog/confirm-dialog.component.spec';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ListService],
})
export class HomeComponent {
  deals: any = [];
  columns: string[] = [
    'Customer',
    'Phone',
    'Email',
    'DealerShip',
    'Lot',
    'HName',
    'BSize',
    'WZone',
    'Attachments',
    'HStatus',
    'LotStatus',
    'Actions',
  ];
  actions: any[];
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(
    private oAuthService: OAuthService,
    private authService: AuthService,
    public readonly list: ListService,
    private houseDealService: HouseDealsService,
    public dialog: MatDialog
  ) {
    this.list.maxResultCount = 2;
  }

  ngOnInit() {
    const dealstreamCreator = () => this.houseDealService.getList();
    const rs = [];
    this.list.hookToQuery(dealstreamCreator).subscribe(response => {
      response.items.forEach(el => {
        this.houseDealService.GetByWorkflowInstanceId(el.instanceId).subscribe(rs => {
          this.houseDealService.GetByWorkflowregistrationId(rs.definitionId).subscribe(def => {
            const activityIds = rs.blockingActivities.map(el => el.activityId);

            this.actions = activityIds.map(actionId => {
              return def.activities.find(t => t.id === actionId);
            });
            el.actions = this.actions;
            this.deals = response;
          });
        });
      });
    });
  }

  createBook() {
    const dialogRef = this.dialog.open(DealDialogComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      panelClass: 'full-screen-modal',
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const { customerName, attachment, boxSize, email, houseName, lot, phone, windZone } =
          result;
        this.houseDealService
          .create(customerName, attachment, boxSize, email, houseName, lot, phone, windZone)
          .subscribe(() => {
            this.list.get();
          });
      }
    });
  }

  triggerAction(formSchema: string, inputProperties: any, instanceId: string) {
    if (!formSchema) {
      this.confirmDialog(inputProperties, instanceId);
    } else {
      const actionsDialogRef = this.dialog.open(ActionsDialogComponent, {
        maxWidth: '100vw',
        maxHeight: '100vh',
        height: '100%',
        width: '100%',
        panelClass: 'full-screen-modal',
        data: {
          formSchema: JSON.parse(formSchema),
        },
      });
      actionsDialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.houseDealService
            .SaveDynamicForm(inputProperties.data.Signal, instanceId, result)
            .subscribe(() => {
              this.list.get();
            });
        }
      });
    }
  }

  confirmDialog(inputProperties: any, instanceId: string): void {
    const message = `Are you sure you want to do this?`;

    const dialogData = new ConfirmDialogModel('Confirm Action', message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: dialogData,
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.houseDealService
          .SaveDynamicForm(inputProperties.data.Signal, instanceId)
          .subscribe(() => {
            this.list.get();
          });
      }
    });
  }

  deleteBook(id: string) {}

  changePage(pageEvent: PageEvent) {
    this.list.page = pageEvent.pageIndex;
  }

  changeSort(sort: Sort) {
    this.list.sortKey = sort.active;
    this.list.sortOrder = sort.direction;
  }

  login() {
    this.authService.navigateToLogin();
  }
}
