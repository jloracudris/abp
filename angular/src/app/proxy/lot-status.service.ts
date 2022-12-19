import type { LotStatusDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LotStatusService {
  apiName = 'Default';
  

  create = (text: string, apiVersion: string = "1.0") =>
    this.restService.request<any, LotStatusDto>({
      method: 'POST',
      url: '/api/app/lot-status',
      params: { text, ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  delete = (id: string, apiVersion: string = "1.0") =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/lot-status/${id}`,
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  getList = (apiVersion: string = "1.0") =>
    this.restService.request<any, PagedResultDto<LotStatusDto>>({
      method: 'GET',
      url: '/api/app/lot-status',
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
