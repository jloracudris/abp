
export interface HouseDealDto {
  instanceId?: string;
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
  isPublished: boolean;
  actions: any[]
}

export interface HouseDto {
  homeId?: string;
  name?: string;
  homeStatusId: number;
  creationTime?: string;
  updateTime?: string;
}

export interface HouseStatusDto {
  id?: string;
  name?: string;
  creationTime?: string;
  updateTime?: string;
}

export interface LotStatusDto {
  id?: string;
  name?: string;
  lotStatusId?: string;
  creationTime?: string;
  updateTime?: string;
}
