parent_path=$(
    cd "$(dirname "${BASH_SOURCE[0]}")"
    pwd -P
)
cd "$parent_path"
# we are in the project root
cd ..

# go to .env and replace postgres to localhost
echo "Modifying .env file..."
sed -i 's/HOST=postgres/HOST=localhost/' .env
echo ".env file is modified"

# find last migration and write new number to variable
echo "Figuring out next revision..."
last_revision=0
for file in "./Migrations"/*; do
    name_wo_ext=${file%.*}
    revision=${name_wo_ext#*_}
    # Verifying that revision is number
    pattern='^[0-9]+$'
    if [[ $revision =~ $pattern ]] ; then
        if ((revision > last_revision)); then
            last_revision=$revision
        fi
    fi
done
next_revision=$(($last_revision + 1))
echo "Next revision will be version ${next_revision}"

# run migration
echo "Running migration..."
dotnet-ef migrations add ${next_revision}
echo "Migration is done"

# return .env file to previous stage
echo "Modifying .env file back..."
sed -i 's/HOST=localhost/HOST=postgres/' .env
echo ".env file is modified back"

echo "Complete!"
