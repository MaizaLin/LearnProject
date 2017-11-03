@echo on
for /f "delims=" %%i in ('dir /ad/b ') do ( 
cd %cd%\%%i
git remote add upstream https://github.com/tuhu/%%i.git
git branch masster
git checkout master
git pull upstream master
git push origin master
cd ..
)