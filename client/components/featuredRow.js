import React, { useEffect, useState } from 'react';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { getFeaturedResturantById } from '../api';
import { themeColors } from '../theme';
import ResturantCard from './resturantCard';


export default function FeatureRow({id, title, description}) {

   const [resturants, setResturants] = useState([]);

  useEffect(() => {
     getFeaturedResturantById(id).then(data=>{
       setResturants(data?.restaurants);
      //  console.log('got data: ',data);
     })
  }, [id])
  //  console.log(resturants);
  
  return (
    <View>
      <View className="flex-row justify-between items-center px-4">
        <View>
          <Text className="font-bold text-lg">{title}</Text>
        </View>
        
        <TouchableOpacity>
          <Text style={{color: themeColors.text}} className="font-semibold">Xem tất cả</Text>
        </TouchableOpacity>
      </View>

      

      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={{
            paddingHorizontal:15,
        }}
        className="overflow-visible py-5"
       >
        {
          resturants && resturants.map(resturant=>{
            return (
                <ResturantCard
                  key={resturant._id}
                  id={resturant._id}
                  imgUrl={resturant.image}
                  title={resturant.name}
                  rating={resturant.rating}
                  type={resturant.type?.name}
                  address={resturant?.address || ''}
                  description={resturant?.description || ''}
                  dishes={resturant?.dishes || []}
                  lng={resturant?.lng || 0}
                  lat={resturant?.lat || 0}
              />    
            )
          })
        }           
       </ScrollView>
    
    </View>
  )
}