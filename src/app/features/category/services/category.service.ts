import { Injectable } from '@angular/core';
import { AddCategoryRequestModels } from '../models/add-category-request.models';
import { Observable, catchError, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Categories } from '../models/categories.model';
import { environment } from 'src/environments/environment.development';
import { UpdateCategoryRequestModels } from '../models/update-category-request.models';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  constructor(
    private http: HttpClient,
  ) { }

  //must return this type to insert anyway
   addCategory(model: AddCategoryRequestModels): Observable<void> {

    //writing post here is important, and ensure the method is also post
    return this.http.post<void>(`${environment.apiBaseUrl}/api/categories`, model);
  }

  //modified this to handle Async Pipes
  getAllCategories(): Observable<Categories[]> {
    return this.http.get<Categories[]>(`${environment.apiBaseUrl}/api/categories`)
      .pipe(
        tap(() => console.log('Successfully fetched categories!')),
        catchError(error => {
          console.error('Error fetching categories:', error);
          return of([]); // return an empty array if there's an error
        })
      );
  }

  getCategoriesById(id: string): Observable<Categories>{
    return this.http.get<Categories>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }

  // implementation for update method

  updateCategory(model: UpdateCategoryRequestModels, id: string): Observable<Categories>{
    return this.http.put<Categories>(`${environment.apiBaseUrl}/api/categories/${id}`, model);
  }

  deleteCategory(id: string): Observable<Categories>{
    return this.http.delete<Categories>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }

}
