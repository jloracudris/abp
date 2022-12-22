import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DealerShipDto } from '@proxy/dto';
import {
  HouseDealDto,
  HouseDto,
  HouseStatusDto,
  LotStatusDto,
} from '@proxy/entities/entities/models';
import { ListService, PagedResultDto } from '@abp/ng.core';
import {
  DealerShipService,
  HouseDealsService,
    HouseService,
  HouseStatusService,
  LotStatusService,
} from '@proxy';

@Component({
  selector: 'app-actions-dialog',
  templateUrl: './actions-dialog.component.html',
  styleUrls: ['./actions-dialog.component.scss'],
  providers: [
    ListService,
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: { hasBackdrop: true, width: '50vw' },
    },
  ],
})
export class ActionsDialogComponent implements OnInit {
  form: FormGroup;
  title: String;
  listSchema: any[];
  displayedColumns: string[];
  file: File = null;
  latLong: string = null;
  lat: number;
  lng: number;

  constructor(
    private fb: FormBuilder,
    public readonly list: ListService,
    @Inject(MAT_DIALOG_DATA) public data: any,    
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({});

    const arrayofForms = [];
    const keys = Object.keys(this.data.formSchema.properties);
    keys.forEach(key => {
      arrayofForms.push(this.data.formSchema.properties[key]);
    });
    this.listSchema = arrayofForms;
    this.title = this.data.formSchema.title;
    this.buildForm();
  }

  onChange(event) {
    this.file = event.target.files[0];
  }

  buildForm() {
    for (let field of this.listSchema) {
      if (field.type === 'houseSpec') {
        this.displayedColumns = field.cols;
      }
      this.form.addControl(field.title, new FormControl());
      if (field.default) {
        this.form.get(field.title).setValue(field.default);
      }
    }
  }

  getLatAndLng($event: any) {
    this.form.get("maps").setValue(`${$event.lat}, ${$event.lng}`);
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
    return this.form.value
  }
}
