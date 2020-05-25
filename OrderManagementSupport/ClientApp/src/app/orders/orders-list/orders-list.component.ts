import { Component, OnInit } from '@angular/core';
import { Order } from '../../shared/model/order';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit {

  orders: Order[];
  items = [];
  pageOfItems: Array<any>;

  constructor(private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.orders = this.route.snapshot.data.orders;
    console.log(this.orders);
    this.items = Array(150).fill(0).map((x, i) => ({ id: (i + 1), name: `Item ${i + 1}`}));
  }

  onChangePage(pageOfItems: Array<any>){
    this.pageOfItems = pageOfItems;
  }
}
