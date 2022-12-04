import { AuditedEntityDto } from "@abp/ng.core";

export interface HouseDealDto extends AuditedEntityDto<string> {
  dealId?: string;
  name?: string;
  phoneNumber?: string;
  email?: string;
  lotNumber?: string;
  houseName?: string;
  boxSize?: string;
  windZone?: string;
  attachment?: string;
  homeStatusId?: string;
  lotStatusId?: string;
  creationTime?: string;
  updateTime?: string;
}

export interface HouseDto {
  homeId?: string;
  name?: string;
  houseStatusId: number;
  creationTime?: string;
  updateTime?: string;
}

export interface HouseStatusDto {
  name?: string;
  creationTime?: string;
  updateTime?: string;
}

export interface LotStatusDto {
  name?: string;
  lotStatusId?: string;
  creationTime?: string;
  updateTime?: string;
}
