import type { HouseStatusDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseStatusService {
  apiName = 'Default';
  

  create = (text: string) =>
    this.restService.request<any, HouseStatusDto>({
      method: 'POST',
      url: '/api/app/house-status',
      params: { text },
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house-status/${id}`,
    },
    { apiName: this.apiName });
  

  getList = () =>
    this.restService.request<any, HouseStatusDto[]>({
      method: 'GET',
      url: '/api/app/house-status',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
