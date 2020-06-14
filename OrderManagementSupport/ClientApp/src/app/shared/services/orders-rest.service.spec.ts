import { TestBed } from '@angular/core/testing';

import { OrdersRestService } from './orders-rest.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import {HttpClientModule} from '@angular/common/http';

describe('OrdersRestService', () => {
  let service: OrdersRestService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(OrdersRestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
