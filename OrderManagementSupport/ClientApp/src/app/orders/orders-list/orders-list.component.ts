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
  pageOfOrders: Array<any>;

  constructor(private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.orders = this.route.snapshot.data.orders;
    console.log(this.orders);
  }

  onChangePage(pageOfOrders: Array<any>){
    this.pageOfOrders = pageOfOrders;
  }
}
