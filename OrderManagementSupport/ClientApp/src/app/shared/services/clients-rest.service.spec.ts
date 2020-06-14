import { TestBed } from '@angular/core/testing';

import { ClientsRestService } from './clients-rest.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import {HttpClientModule} from '@angular/common/http';

describe('ClientsRestService', () => {
  let service: ClientsRestService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(ClientsRestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
