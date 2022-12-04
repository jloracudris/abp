import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { ListService, PagedResultDto } from "@abp/ng.core";
import { HouseDealDto } from '@proxy/entities/entities';
import { HouseDealsService } from '@proxy/';
import { PageEvent } from "@angular/material/paginator";
import { Sort } from "@angular/material/sort";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ListService]
})
export class HomeComponent {
  deals = { items: [], totalCount: 0 } as PagedResultDto<HouseDealDto>;
  columns: string[] = ["Customer Name", "Phone Number", "Email", "DealerShip", "Lot #", "Home Name", "Box Size", "Wind Zone", "Attachments", "Home Status", "Lot Status", "Actions"];

  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService,
    private authService: AuthService, 
    public readonly list: ListService, 
    private houseDealService: HouseDealsService) {
      this.list.maxResultCount = 2;
  }

  ngOnInit() {
    const dealstreamCreator = () => this.houseDealService.getList();

    this.list.hookToQuery(dealstreamCreator).subscribe((response) => {
      this.deals = response;
    });
  }

  changePage(pageEvent: PageEvent) {
    this.list.page = pageEvent.pageIndex;
  }

  changeSort(sort: Sort) {
    this.list.sortKey = sort.active;
    this.list.sortOrder = sort.direction;
  }

  login() {
    this.authService.navigateToLogin();
  }
}
