import type { HouseDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseService {
  apiName = 'Default';
  

  create = (text: string, apiVersion: string = "1.0") =>
    this.restService.request<any, HouseDto>({
      method: 'POST',
      url: '/api/app/house',
      params: { text, ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  delete = (id: string, apiVersion: string = "1.0") =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house/${id}`,
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  getList = (apiVersion: string = "1.0") =>
    this.restService.request<any, PagedResultDto<HouseDto>>({
      method: 'GET',
      url: '/api/app/house',
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
