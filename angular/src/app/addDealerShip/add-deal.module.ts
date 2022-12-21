import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
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
    ReactiveFormsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AddDealerShipModule {}
