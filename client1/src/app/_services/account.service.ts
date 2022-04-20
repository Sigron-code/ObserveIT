import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService { 
  baseUrl = 'https://localhost:5001/';

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'users/users', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          //this.currentUserSource.next(user);
        }
      })
    )
  }

  
  logout(){
    localStorage.removeItem('user');
   // this.currentUserSource.next(null);
  }

}
