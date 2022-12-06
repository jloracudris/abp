import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MAT_DIALOG_DEFAULT_OPTIONS, } from "@angular/material/dialog";
import { FormBuilder, FormGroup, Validators } from"@angular/forms";
import { DealerShipDto } from '@proxy/dto';
import { HouseDealDto, HouseDto, HouseStatusDto, LotStatusDto } from '@proxy/entities/entities/models';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { DealerShipService, HouseService, HouseStatusService, LotStatusService } from '@proxy';

@Component({
  selector: 'app-deal-dialog',
  templateUrl: './deal-dialog.component.html',
  styleUrls: ['./deal-dialog.component.scss'],
  providers: [
    ListService, 
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: { hasBackdrop: true, width:"50vw" },
    }
  ]
})
export class DealDialogComponent implements OnInit {
  form: FormGroup
  dealership = { items: [], totalCount: 0 } as PagedResultDto<DealerShipDto>;
  house = { items: [], totalCount: 0 } as PagedResultDto<HouseDto>;
  lotStatus = { items: [], totalCount: 0 } as PagedResultDto<LotStatusDto>;
  houseStatus = { items: [], totalCount: 0 } as PagedResultDto<HouseStatusDto>;

  constructor(
    private fb: FormBuilder, 
    public readonly list: ListService,
    @Inject(MAT_DIALOG_DATA) public data: HouseDealDto,
    private dealerShipService: DealerShipService,
    private houseService: HouseService,
    private lotService: LotStatusService,
    private houseStatusService: HouseStatusService) { }

  ngOnInit(): void {

    const dealstreamCreator = () => this.dealerShipService.getList();

    this.list.hookToQuery(dealstreamCreator).subscribe((response) => {
      this.dealership = response;
    });

    const houseStreamCreator = () => this.houseService.getList();

    this.list.hookToQuery(houseStreamCreator).subscribe((response) => {
      this.house = response;
    });

    const lotStatusStreamCreator = () => this.lotService.getList();

    this.list.hookToQuery(lotStatusStreamCreator).subscribe((response) => {
      this.lotStatus = response;
    });

    const houseStatusStreamCreator = () => this.houseStatusService.getList();

    this.list.hookToQuery(houseStatusStreamCreator).subscribe((response) => {
      this.houseStatus = response;
    });

    this.buildForm();
  }

  buildForm() {
    this.form = this.fb.group({
      customerName: [null, Validators.required],
      phone: [null, Validators.required],
      email: [null, Validators.required],
      dealerShip: [null, null],
      lot: [null, null],
      houseName: [null, Validators.required],
      boxSize: [null, null],
      windZone: [null, null],
      attachment: [null, null],
      houseStatus: [null, Validators.required],
      lotStatus: [null, Validators.required],
    });
  }

  getFormValue() {
    return this.form.value;
  }
}
