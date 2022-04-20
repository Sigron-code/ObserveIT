import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ObserveIT';
  users: any;
  userId: any;
  heartBit:any;
  unregistered_agents:any;
  registered_agents :any;

  constructor(private http: HttpClient){ }

  ngOnInit() {
      this.getUsers();
  }  

  getUsers() {
    this.http.get('https://localhost:5001/api/users/users').subscribe(response => {
       this.users = response;
     }, error => {
       console.log(error);
     })
  }

  Heartbeats() {

    var url = 'https://localhost:5001/api/users/heartbeats'+'?id='+this.userId;
    this.http.get(url).subscribe(response => {
      this.heartBit = response;
    }, error => {
      console.log(error);
    })

   
    console.log(this.heartBit);
  }

  RegisteredAgents(){
    var url = 'https://localhost:5001/api/users/registered_agents';
    this.http.get(url).subscribe(response => {
      this.registered_agents = response;
    }, error => {
      console.log(error);
    })
   
    console.log(this.registered_agents);
  }

  UnRegisteredAgents(){
    var url = 'https://localhost:5001/api/users/registered_agents';
    this.http.get(url).subscribe(response => {
      this.unregistered_agents = response;
    }, error => {
      console.log(error);
    })
   
    console.log(this.registered_agents);
  }


}
