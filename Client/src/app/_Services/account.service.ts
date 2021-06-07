import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseurl='https://localhost:5001/api/'

  constructor(private http: HttpClient) { }
  login(model : any){
    return this.http.post(this.baseurl+"Account/login",model);
  }
}
