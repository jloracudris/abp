import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { DealDialogComponent } from './components/deal-dialog/deal-dialog.component';
import { ActionsDialogComponent } from './components/actions-dialog/actions-dialog.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component.spec';
import { MapsComponent } from './components/map/map.component';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  declarations: [
    HomeComponent,
    DealDialogComponent,
    ActionsDialogComponent,
    ConfirmDialogComponent,
    MapsComponent,
  ],
  imports: [
    SharedModule,
    HomeRoutingModule,
    AgmCoreModule.forRoot({
      apiKey: '',
    }),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class HomeModule {}
