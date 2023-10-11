import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Categories } from '../models/categories.model';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {

  // import the necessary model first, in ur service u called the model so over here u need it as well
  categories$?: Observable<Categories[]>

  showTable: boolean = false; // Initialize to true if you want the table to be visible initially


  // register it in the constructor using depedency injection to call it later
  constructor(
    private categoryService: CategoryService
  ){}

  // dont write implementation here, thats just lazy and stupid.
  ngOnInit(): void {
    this.fetchCategories();
  }

  // actual implementation
  fetchCategories(): void {
    this.toggleTable();
    this.categories$ = this.categoryService.getAllCategories()
  }

  // to hide and show table
  toggleTable() {
    this.showTable = !this.showTable;
  }

}
