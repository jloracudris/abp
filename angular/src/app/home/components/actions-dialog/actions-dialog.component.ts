import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MAT_DIALOG_DEFAULT_OPTIONS, } from "@angular/material/dialog";
import { FormBuilder, FormControl, FormGroup, Validators } from"@angular/forms";
import { DealerShipDto } from '@proxy/dto';
import { HouseDealDto, HouseDto, HouseStatusDto, LotStatusDto } from '@proxy/entities/entities/models';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { DealerShipService, HouseDealsService, HouseService, HouseStatusService, LotStatusService } from '@proxy';

@Component({
  selector: 'app-actions-dialog',
  templateUrl: './actions-dialog.component.html',
  styleUrls: ['./actions-dialog.component.scss'],
  providers: [
    ListService, 
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: { hasBackdrop: true, width:"50vw" },
    }
  ]
})
export class ActionsDialogComponent implements OnInit {
  form: FormGroup
  listSchema: any[];

  constructor(
    private fb: FormBuilder, 
    public readonly list: ListService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private houseDealService: HouseDealsService) { }

  ngOnInit(): void {
    this.form = this.fb.group({});
    console.log(this.data.method)
    this.houseDealService.GetSchemaActivity(this.data.url, this.data.method).subscribe({
      next: listFormDefinition => {
        const arrayofForms = [];
        const keys = Object.keys(listFormDefinition.properties)
        keys.forEach((key) => {
          arrayofForms.push(listFormDefinition.properties[key])
        });
        this.listSchema = arrayofForms;
        this.buildForm();
      },
    });
  }

  buildForm() {
    for (let field of this.listSchema) {
      this.form.addControl(field.title, new FormControl());
    }
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    console.log(this.form.value);
    /* this.approvalService.create(this.form.value).subscribe(response => {
      this.sendActivity(response).subscribe({
        next: (response: any) => {
          this.isModalOpen = false;
          this.form.reset();
          this.list.get();
        },
        error: error => {
          console.log(error);
        },
      });
    }); */
  }

  getFormValue() {
    return this.form.value;
  }
}
