import type { HouseDealDto } from './entities/entities/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HouseDealsService {
  apiName = 'Default';


  create = (name: string, attachment: string, boxsize: string, email: string, houseName: string, lotNumber: string, phoneNumber: string, windZone: string, dealerShip, houseStatus, lotStatus, apiVersion: string = "1.0") =>
    this.restService.request<any, HouseDealDto>({
      method: 'POST',
      url: '/v2/house-deals',
      body: { name, attachment, boxsize, email, houseName, lotNumber, phoneNumber, windZone, dealerShip, houseStatus, lotStatus, ["api-version"]: apiVersion },
    },
      { apiName: this.apiName });


  delete = (id: string, apiVersion: string = "1.0") =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/house-deals/${id}`,
      params: { ["api-version"]: apiVersion },
    },
      { apiName: this.apiName });


  getList = (apiVersion: string = "1.0") =>
    this.restService.request<any, PagedResultDto<HouseDealDto>>({
      method: 'GET',
      url: '/api/app/house-deals',
      params: { ["api-version"]: apiVersion },
    },
      { apiName: this.apiName });

  constructor(private restService: RestService) { }

  GetSchemaActivity = (url: string, method: string) =>
    this.restService.request<any, any>({
      method,
      url
    },
      { apiName: this.apiName });

  GetByWorkflowInstanceId = (instanceId: string) =>
    this.restService.request<any, any>({
      method: 'GET',
      url: `/v1/workflow-instances/${instanceId}`,
    },
      { apiName: this.apiName });

  GetByWorkflowregistrationId = (registerId: string) =>
    this.restService.request<any, any>({
      method: 'GET',
      url: `/v1/workflow-registry/${registerId}`,
    },
      { apiName: this.apiName });

  SaveDynamicForm = (signal: string, workflowInstanceId: string, formValues: any = null) =>
    this.restService.request<any, any>({
      method: 'POST',
      url: `/custom-signals/${signal}/execute`,
      body: { workflowInstanceId, ...formValues }
    },
      { apiName: this.apiName });
}
