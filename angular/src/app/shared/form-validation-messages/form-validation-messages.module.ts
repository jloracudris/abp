import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared.module';
import { FormValidationMessagesComponent } from './form-validation-messages.component';

@NgModule({
  declarations: [
    FormValidationMessagesComponent,
  ],
  imports: [
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    FormValidationMessagesComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class FormValidationMessagesModule {}
