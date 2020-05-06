import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client } from '../model/client';
import { Order } from "../model/order";

@Injectable({
  providedIn: 'root'
})

export class ClientsRestService {

  constructor(private readonly http: HttpClient) {

  }

  findAll(): Observable<Client[]> {
    return this.http.get<Client[]>('/api/clients');
  }

  findBookById(id: number): Observable<Client> {
    return this.http.get<Client>(`/api/clients/${id}`);
  }

  delete(clientId: number): Observable<Client> {
    return this.http.delete<Client>(`/api/clients/${clientId}`);
  }
}
