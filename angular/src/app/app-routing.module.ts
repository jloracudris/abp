import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'deals',
    pathMatch: 'full',
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'Americasa_Deals_View',
    },
    loadChildren: () => import('./dealership/dealership.module').then(m => m.DealerShipModule),
  },
  {
    path: 'add-deals',
    canActivate: [PermissionGuard],
    pathMatch: 'full',
    loadChildren: () => import('./addDealerShip/add-deal.module').then(m => m.AddDealerShipModule),
    data: {
      requiredPolicy: 'Americasa_Deals_Create',
    },
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
