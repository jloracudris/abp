import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DealerShipComponent } from './dealership.component';

const routes: Routes = [{ path: '', component: DealerShipComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DealerShipRoutingModule {}
