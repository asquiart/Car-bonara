import { Component, OnDestroy, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable, Subscription } from 'rxjs';
import { filter, map, shareReplay, withLatestFrom } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { NavigationEnd, Router } from '@angular/router';
import { MatSidenav } from '@angular/material/sidenav';
import { Routing } from 'src/app/app-routing.module';

@Component({
  selector: 'material-design-main',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnDestroy {

  @ViewChild('drawer', { static: true }) drawer?: MatSidenav;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
    
  private routingEventSubscription : Subscription;

  constructor(
    private breakpointObserver: BreakpointObserver,
    public authenticationService: AuthenticationService,
    private router: Router
  ) {

    this.routingEventSubscription = this.router.events.subscribe(event => {
      // close sidenav on routing
      this.drawer?.close();
    });

    
    this.authenticationService.loggedInEvent.subscribe(loggedIn => {
      console.debug("Logged in");
      this.router.navigate([this.getStartpage()]);
    });

    this.authenticationService.loggedOutEvent.subscribe(loggedOut => {
      console.debug("Logged out");
      this.router.navigate([Routing.ROUTE_ROOT]);
    });
  }


  ngOnDestroy(): void {
    this.routingEventSubscription.unsubscribe();
  }

  clickUser() {
  }

  logout()
  {
    this.authenticationService.logout();
  }

  getStartpage()
  {
    return NavigationComponent.getStartpage(this.authenticationService);
  }

  static getStartpage(authenticationService: AuthenticationService) : string {
    if (!authenticationService.isLoggedIn())
    {
      return Routing.ROUTE_ROOT;
    } else if (!authenticationService.isEmployee())
    {
      return Routing.ROUTE_OVERVIEW;
    } 
    else return Routing.ROUTE_EMPLOYEE;
  }
}
