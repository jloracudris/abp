import type { HouseDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseService {
  apiName = 'Default';
  

  create = (text: string) =>
    this.restService.request<any, HouseDto>({
      method: 'POST',
      url: '/api/app/house',
      params: { text },
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house/${id}`,
    },
    { apiName: this.apiName });
  

  getList = () =>
    this.restService.request<any, HouseDto[]>({
      method: 'GET',
      url: '/api/app/house',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
