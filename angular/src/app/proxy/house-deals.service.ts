import type { HouseDealDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseDealsService {
  apiName = 'Default';


  create = (name: string, attachment: string , boxsize: string , email: string , houseName: string, lotNumber: string , phone: string , windZone: string ) =>
    this.restService.request<any, HouseDealDto>({
      method: 'POST',
      url: '/api/app/house-deals',
      params: { name,  attachment, boxsize, email, houseName, lotNumber, phone, windZone},
    },
      { apiName: this.apiName });


  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house-deals/${id}`,
    },
      { apiName: this.apiName });


  GetSchemaActivity = (apiVersion: string = "1.0") =>
    this.restService.request<any, any>({
      method: 'GET',
      url: `/start-home-review/start`,
    },
      { apiName: this.apiName });


  getList = () =>
    this.restService.request<any, PagedResultDto<HouseDealDto>>({
      method: 'GET',
      url: '/api/app/house-deals',
    },
      { apiName: this.apiName });

  constructor(private restService: RestService) { }
}
