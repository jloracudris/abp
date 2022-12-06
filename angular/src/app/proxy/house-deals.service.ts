import type { HouseDealDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseDealsService {
  apiName = 'Default';
  

  create = (text: string) =>
    this.restService.request<any, HouseDealDto>({
      method: 'POST',
      url: '/api/app/house-deals',
      params: { text },
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house-deals/${id}`,
    },
    { apiName: this.apiName });
  

  getList = () =>
    this.restService.request<any, PagedResultDto<HouseDealDto>>({
      method: 'GET',
      url: '/api/app/house-deals',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
