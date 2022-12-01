
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
