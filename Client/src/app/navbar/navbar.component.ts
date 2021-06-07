import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './../_Services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: any = {}
  LoggedIn: boolean = false;
  constructor(private accountservice: AccountService) { }

  ngOnInit(): void {
  }

  login(){
    this.accountservice.login(this.model).subscribe(response =>
      {console.log(response);
      this.LoggedIn =true;},
      error =>
      {
        console.log(error);
      }
      )
  }

  logout(){
    this.LoggedIn = false;
  }

}
