import type { DealerShipDto } from './dto/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DealerShipService {
  apiName = 'Default';
  

  create = (text: string, apiVersion: string = "1.0") =>
    this.restService.request<any, DealerShipDto>({
      method: 'POST',
      url: '/api/app/dealer-ship',
      params: { text, ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  delete = (id: string, apiVersion: string = "1.0") =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/dealer-ship/${id}`,
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });
  

  getList = (apiVersion: string = "1.0") =>
    this.restService.request<any, PagedResultDto<DealerShipDto>>({
      method: 'GET',
      url: '/api/app/dealer-ship',
      params: { ["api-version"]: apiVersion },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
