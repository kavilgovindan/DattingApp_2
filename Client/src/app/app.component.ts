import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  Users: any;

  constructor(private httpRequest: HttpClient){}
  ngOnInit() {
    this.getUsers();
  }
  getUsers(){
    this.httpRequest.get('https://localhost:5001/api/Users').subscribe(response =>{
      this.Users = response;
    },error=>{
      console.log(error);
      
    }) 
   }
}
