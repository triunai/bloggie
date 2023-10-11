import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequestModels } from '../models/add-category-request.models';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnDestroy {

  model: AddCategoryRequestModels;
  private addCategorySubscription?: Subscription ;

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.model = {
      name: '',
      urlHandle: ''
    };
  }


  //void type by default
  onFormSubmit(){
    console.log(this.model);
    this.addCategorySubscription = this.categoryService.addCategory(this.model).subscribe({
      next: (response) => {
          console.log("Successful add" +this.model.name);
          this.router.navigateByUrl('/admin/categories');
      },
      error: (error) => {
        console.log('Error, model wasnt added or data wasnt bound properly');
    }
  });

  }

  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }
}
