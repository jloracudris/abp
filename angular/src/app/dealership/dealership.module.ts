import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { DealerShipRoutingModule } from './dealership-routing.module';
import { DealerShipComponent } from './dealership.component';

@NgModule({
  declarations: [DealerShipComponent],
  imports: [SharedModule, DealerShipRoutingModule],
})
export class DealerShipModule {}
