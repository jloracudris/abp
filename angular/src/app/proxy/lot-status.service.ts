import type { LotStatusDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LotStatusService {
  apiName = 'Default';
  

  create = (text: string) =>
    this.restService.request<any, LotStatusDto>({
      method: 'POST',
      url: '/api/app/lot-status',
      params: { text },
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/lot-status/${id}`,
    },
    { apiName: this.apiName });
  

  getList = () =>
    this.restService.request<any, PagedResultDto<LotStatusDto>>({
      method: 'GET',
      url: '/api/app/lot-status',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
