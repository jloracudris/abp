import type { HouseStatusDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseStatusService {
  apiName = 'Default';
  

  create = (text: string, apiVersion: string = "1.0") =>
    this.restService.request<any, HouseStatusDto>({
      method: 'POST',
      url: '/api/app/house-status',
      params: { text, ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  delete = (id: string, apiVersion: string = "1.0") =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house-status/${id}`,
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  getList = (apiVersion: string = "1.0") =>
    this.restService.request<any, PagedResultDto<HouseStatusDto>>({
      method: 'GET',
      url: '/api/app/house-status',
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
