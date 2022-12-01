import type { DealerShipDto } from './dto/models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DealerShipService {
  apiName = 'Default';
  

  create = (text: string) =>
    this.restService.request<any, DealerShipDto>({
      method: 'POST',
      url: '/api/app/dealer-ship',
      params: { text },
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/dealer-ship/${id}`,
    },
    { apiName: this.apiName });
  

  getList = () =>
    this.restService.request<any, DealerShipDto[]>({
      method: 'GET',
      url: '/api/app/dealer-ship',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
