import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ClientsRestService {

  constructor(private readonly http: HttpClient) {

  }
}
