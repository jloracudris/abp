import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { ListService, PagedResultDto } from "@abp/ng.core";
import { HouseDealDto } from '@proxy/entities/entities';
import { PageEvent } from "@angular/material/paginator";
import { Sort } from "@angular/material/sort";
import { HouseDealsService } from '@proxy/house-deals.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ListService]
})
export class HomeComponent {
  deals = { items: [], totalCount: 0 } as PagedResultDto<HouseDealDto>;
  columns: string[] = ["Customer", "Phone", "Email", "DealerShip", "Lot", "HName", "BSize", "WZone", "Attachments", "HStatus", "LotStatus", "Actions"];

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

  editBook(id: string) {
    /* this.houseDealService.get(id).subscribe((book) => {
        const dialogRef = this.dialog.open(BookDialogComponent, {
            data: book
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.bookService.update(id, result).subscribe(() => {
                    this.list.get();
                });
            }
        });
    }); */
  }

  deleteBook(id: string) {} 

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
