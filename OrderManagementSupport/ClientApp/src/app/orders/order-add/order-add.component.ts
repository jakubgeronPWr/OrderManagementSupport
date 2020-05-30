import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-order-add',
  templateUrl: './order-add.component.html',
  styleUrls: ['./order-add.component.css']
})
export class OrderAddComponent implements OnInit {

  constructor() { }

  orderDate = new Date();
  realizationDate = new Date();

  ngOnInit(): void {
    this.realizationDate.setDate(this.realizationDate.getDate() + 3);
  }

}
