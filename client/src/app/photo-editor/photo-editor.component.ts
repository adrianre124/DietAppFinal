import { Component, OnInit, inject, input, output } from '@angular/core';
import { FileUploadModule, FileUploader } from 'ng2-file-upload';
import { environment } from '../../environments/environment.development';
import { Product } from '../_models/product';
import { ProductService } from '../_services/product.service';
import { AccountService } from '../_services/account.service';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [FileUploadModule, NgIf, NgFor ,NgClass, NgStyle, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit{
  private accountService = inject(AccountService);
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  fileChange = output<File>();
  imagePreviewUrl: string | ArrayBuffer | null = null;

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'product/add-image',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 // 10MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      if (this.uploader!.queue.length > 1) {
        this.uploader?.removeFromQueue(this.uploader.queue[0]);
      }
      file.withCredentials = false;
      this.previewIamge(file._file);
      this.fileChange.emit(file._file);
    }
  }

  previewIamge(file: File) {
    const reader = new FileReader();
    reader.onload = (event: any) => {
      this.imagePreviewUrl = event.target.result;
    };
    reader.readAsDataURL(file);
  }
}
