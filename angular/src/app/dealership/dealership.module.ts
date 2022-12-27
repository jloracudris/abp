import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { DealerShipRoutingModule } from './dealership-routing.module';
import { DealerShipComponent } from './dealership.component';
import { DealDialogComponent } from './components/deal-dialog/deal-dialog.component';
import { ActionsDialogComponent } from './components/actions-dialog/actions-dialog.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component.spec';
import { MapsComponent } from './components/map/map.component';
import { AgmCoreModule } from '@agm/core';
import { FormValidationMessagesModule } from '../shared/form-validation-messages/form-validation-messages.module';
import { DisableActionDirective } from '../shared/directives/disable-action.directive';

@NgModule({
  declarations: [
    DealerShipComponent,
    DealDialogComponent,
    ActionsDialogComponent,
    ConfirmDialogComponent,
    DisableActionDirective,
    MapsComponent
  ],
  imports: [
    SharedModule,
    DealerShipRoutingModule,
    FormValidationMessagesModule,
    AgmCoreModule.forRoot({
      apiKey: '',
    }),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DealerShipModule {}
