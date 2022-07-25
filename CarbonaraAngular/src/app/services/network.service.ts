import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { interval, Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NetworkService {

  static PING_INTERVAL = 5000;

  public online: boolean = true;
  private pingUrl: string = "/api/ping";
  private passedTime: number = 0;

  public networkOnlineAgainEvent = new EventEmitter<any>();
  public networkOfflineEvent = new EventEmitter<any>();

  constructor(
    private http: HttpClient
  ) {
    
    interval(NetworkService.PING_INTERVAL).subscribe(() => {
      this.passedTime += NetworkService.PING_INTERVAL;
      this.ping();
    });
  }


  private ping() {

    let sub : Subscription = this.http.get(this.pingUrl).subscribe({
      next: (info) => {
        if (!this.online)
        {
          console.warn("Online again after " + this.passedTime + "ms");
          this.networkOnlineAgainEvent.emit(this.passedTime);
          this.passedTime = 0;
        }
        this.online = true;
      },
      error: (error: HttpErrorResponse) => {
        if (this.online)
        {
          console.warn("Offline");
          this.networkOfflineEvent.emit(this.passedTime);
          this.passedTime = 0;
        }
        this.online = false;
      }, complete: () => sub.unsubscribe()
    });

  }
}
