import { TestBed } from '@angular/core/testing';

import { ClientsRestService } from './clients-rest.service';

describe('ClientsRestService', () => {
  let service: ClientsRestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientsRestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
