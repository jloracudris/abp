import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { ListService, PagedResultDto } from "@abp/ng.core";
import { HouseDealDto } from '@proxy/entities/entities';
import { PageEvent } from "@angular/material/paginator";
import { Sort } from "@angular/material/sort";
import { HouseDealsService } from '@proxy/house-deals.service';
import { MatDialog } from '@angular/material/dialog';
import { DealDialogComponent } from './components/deal-dialog/deal-dialog.component';
import { ActionsDialogComponent } from './components/actions-dialog/actions-dialog.component';

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
    private houseDealService: HouseDealsService,
    public dialog: MatDialog) {
    this.list.maxResultCount = 2;
  }

  ngOnInit() {
    const dealstreamCreator = () => this.houseDealService.getList();

    this.list.hookToQuery(dealstreamCreator).subscribe((response) => {
      this.deals = response;
    });
  }

  createBook() {
    const dialogRef = this.dialog.open(DealDialogComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      panelClass: 'full-screen-modal'
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const { customerName, attachment, boxSize, email, houseName, lot, phone, windZone} = result;
        this.houseDealService.create(customerName, attachment, boxSize, email, houseName, lot, phone, windZone).subscribe(() => {
          this.list.get();
        });
      }
    });
  }

  triggerAction() {
    this.dialog.open(ActionsDialogComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      panelClass: 'full-screen-modal'
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
