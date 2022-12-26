import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormValidationMessagesComponent } from '../shared/form-validation-messages/form-validation-messages.component';
import { FormValidationMessagesModule } from '../shared/form-validation-messages/form-validation-messages.module';
import { SharedModule } from '../shared/shared.module';
import { AddDealRoutingModule } from './add-deal-routing.module';
import { AddDealComponent } from './add-deal.component';

@NgModule({
  declarations: [
    AddDealComponent,
  ],
  imports: [
    SharedModule,
    AddDealRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    FormValidationMessagesModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AddDealerShipModule {}
