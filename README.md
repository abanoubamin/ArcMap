# ArcMap
## Project Description
The application allow user to:
1. Load map documents from local hard drivers (.mxd files)
2. Navigate between different dataframes within map document (Map must be changed according to selected dataframe)
3. Display the layers of the current dataframe with the option of turning on/off any layer as ArcMap.
4. Add new layer from geodatabase / remove an existing layer from the map.
5. Navigate through the map (Zoom In/Zoom Out/Full Extent).
6. Make a defination query on a layer based on the expression that will be written by the user.
7. display the current location (x, y) on a status bar while moving around the map.
8. Attribute Query (Select by Attribute) the user must be able to:

        a. Select features from different layers based on where clause (selection on map)
        1. Display the fields of the selected layer and the unique values of the selected field as a help for the user for writing the query. (query builder in arcmap)
        2. Define the selection method. (add to current selection, create new selection, remove from current selection)
        b. Display the attributes of the selected features. (for example, in a datagridview)
9. Spatial Query (Select by Location) – the user must be able to:
        
        a. Use the current selection (output of the select by attribute function) to select features from any layer based on spatial relationship between the features in this layer and the selected features. Example on SouthAmerica database: we want to select the cities that are located inside the countries that have population > 100000. In this example, user must be able to use select by attributes tool to select countries with population > 100000 then selects the cities that are located inside these countries using select by location).
        b. Clear the selection.
