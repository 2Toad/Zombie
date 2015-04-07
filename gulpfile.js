var gulp = require('gulp'),
    plumber = require('gulp-plumber'),
    bower = require('gulp-bower'),
    sass = require('gulp-sass'),
    browserSync = require('browser-sync');

gulp.task('bower-update', function () {
    return bower({
        'directory': 'bower_components',
        'cmd': 'update'
    });
});

gulp.task('bower-copy', ['bower-update'], function () {
    gulp.src([
        'bower_components/jquery/dist/jquery.js',
        'bower_components/lodash/lodash.js',
        'bower_components/toastr/toastr.js',
        'bower_components/ladda/js/ladda.js',
        'bower_components/ladda/js/spin.js',
        'bower_components/angular/angular.js',
        'bower_components/angular-route/angular-route.js',
        'bower_components/angular-cookies/angular-cookies.js',
        'bower_components/angular-sanitize/angular-sanitize.js',
        'bower_components/angular-i18n/angular-locale_en-us.js',
        'bower_components/angular-i18n/angular-locale_es.js',
        'bower_components/bootstrap/dist/js/bootstrap.js',
        'bower_components/angular-bootstrap/ui-bootstrap.js',
        'bower_components/angular-bootstrap/ui-bootstrap-tpls.js',
        'bower_components/angular-localizer/angular-localizer.js',
        'bower_components/angular-errorz/angular-errorz.js',
        'bower_components/barricade-angular/barricade-angular.js',
        'bower_components/xtform/dist/xtForm.js',
        'bower_components/angular-local-storage/dist/angular-local-storage.js'
    ])
    .pipe(gulp.dest('Zombie/app/js/vendor'));

    gulp.src('bower_components/ladda/css/ladda.scss')
        .pipe(gulp.dest('Zombie/app/sass/vendor'));

    gulp.src([
        'bower_components/toastr/toastr.css',
        'bower_components/bootstrap/dist/css/bootstrap.css',
        'bower_components/font-awesome/css/font-awesome.css'
    ])
    .pipe(gulp.dest('Zombie/app/css/vendor'));

    gulp.src('bower_components/font-awesome/fonts/*.*')
        .pipe(gulp.dest('Zombie/app/css/fonts'));
});

gulp.task('sass', function () {
    gulp.src('Zombie/app/sass/app.scss')
        .pipe(plumber())
        .pipe(sass())
        .pipe(gulp.dest('Zombie/app/css'));
});

gulp.task('watch', function () {
    gulp.watch('Zombie/app/sass/*.scss', ['sass']);
});

gulp.task('browser-sync', function () {
    var files = [
       'Zombie/app/js/**/*.js',
       'Zombie/app/css/app.css',
       'Zombie/app/views/**/*.html',
       'Zombie/bin/Zombie.dll'
    ];

    browserSync.init(files, {
        proxy: 'localhost:65432'
    });
});

gulp.task('bower', ['bower-update', 'bower-copy']);
gulp.task('default', ['browser-sync', 'watch']);
