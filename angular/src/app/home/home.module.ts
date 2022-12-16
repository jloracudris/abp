import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { DealDialogComponent } from './components/deal-dialog/deal-dialog.component';
import { ActionsDialogComponent } from './components/actions-dialog/actions-dialog.component';

@NgModule({
  declarations: [HomeComponent, DealDialogComponent, ActionsDialogComponent],
  imports: [SharedModule, HomeRoutingModule],
})
export class HomeModule {}
